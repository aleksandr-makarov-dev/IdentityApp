import {
  FC,
  PropsWithChildren,
  createContext,
  useEffect,
  useState,
  useContext,
  useRef,
} from "react";
import { Profile } from "../types";
import { useRefreshToken } from "../api/refreshToken";
import { jwtDecode } from "jwt-decode";

const jwtTokenKey = "jwt-token";

type SessionContextData = {
  isAuthenticated: boolean;
  profile: Profile | null;
  isLoading?: boolean;
  signIn: (token: string) => void;
  signOut: () => void;
};

const SessionContext = createContext<SessionContextData | null>(null);

const SessionProvider: FC<PropsWithChildren> = ({ children }) => {
  const { mutate: refreshToken } = useRefreshToken();
  const timeoutRef = useRef<ReturnType<typeof setTimeout> | null>(null);

  const [token, setToken] = useState<string | null>(
    localStorage.getItem(jwtTokenKey)
  );

  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
  const [profile, setProfile] = useState<Profile | null>(null);

  useEffect(() => {
    if (token) {
      validateToken(token);
    } else {
      tryRefreshToken();
    }

    return () => {
      if (timeoutRef.current) {
        clearTimeout(timeoutRef.current);
      }
    };
  }, [token]);

  const validateToken = (token: string) => {
    try {
      const decodedToken = jwtDecode<Profile>(token);
      const currentDate = new Date();
      const expiresAt = (decodedToken.exp ?? 0) * 1000;
      const isExpired = expiresAt < currentDate.getTime();

      if (isExpired) {
        tryRefreshToken();
      } else {
        setProfile(decodedToken);
        setIsAuthenticated(true);
        scheduleTokenRefresh(expiresAt - currentDate.getTime());
      }
    } catch (e) {
      console.error("Invalid token", e);
      signOut();
    }
  };

  const scheduleTokenRefresh = (msUntilExpiry: number) => {
    if (timeoutRef.current) {
      clearTimeout(timeoutRef.current);
    }

    timeoutRef.current = setTimeout(() => {
      tryRefreshToken();
    }, msUntilExpiry - 60000);
  };

  const tryRefreshToken = () => {
    refreshToken(null, {
      onSuccess: ({ token }) => {
        signIn(token);
      },
      onError: () => {
        signOut();
      },
    });
  };

  const signIn = (token: string) => {
    localStorage.setItem(jwtTokenKey, token);
    setToken(token);
  };

  const signOut = () => {
    if (timeoutRef.current) {
      clearTimeout(timeoutRef.current);
    }
    setToken(null);
    setIsAuthenticated(false);
    setProfile(null);
    localStorage.removeItem(jwtTokenKey);
  };

  return (
    <SessionContext.Provider
      value={{
        isAuthenticated,
        profile,
        signIn,
        signOut,
      }}
    >
      {children}
    </SessionContext.Provider>
  );
};

export const useSession = () => {
  const context = useContext(SessionContext);

  if (context === null) {
    throw new Error(
      "useSession hook can only be used within 'SessionProvider'"
    );
  }

  return context;
};

export default SessionProvider;
