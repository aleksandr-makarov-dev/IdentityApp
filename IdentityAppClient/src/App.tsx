import "./index.css";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { AppRouterProvider } from "./providers/AppRouterProvider";
import SessionProvider from "./features/accounts/providers/SessionProvider";

function App() {
  const client = new QueryClient({
    defaultOptions: {
      queries: {
        refetchInterval: false,
        refetchOnWindowFocus: false,
        refetchOnReconnect: false,
        retry: false,
      },
    },
  });

  return (
    <QueryClientProvider client={client}>
      <SessionProvider>
        <AppRouterProvider />
      </SessionProvider>
    </QueryClientProvider>
  );
}

export default App;
