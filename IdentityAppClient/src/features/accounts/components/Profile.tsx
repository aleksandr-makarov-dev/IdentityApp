import { Button, Divider, Menu, MenuItem, Popover } from "@blueprintjs/core";
import { FC } from "react";
import { useRefreshToken } from "../api/refreshToken";
import { useSession } from "../providers/SessionProvider";

const Profile: FC = () => {
  const { signIn, signOut } = useSession();
  const { mutate } = useRefreshToken();

  const handleRefreshSession = () => {
    mutate(undefined, {
      onSuccess: ({ token }) => {
        console.log("token:", token);
        signIn(token);
      },
      onError: (e) => {
        console.log(e.message);
        signOut();
      },
    });
  };

  return (
    <Popover
      position="bottom-left"
      content={
        <Menu>
          <MenuItem
            icon="new-text-box"
            text="Refresh session"
            onClick={handleRefreshSession}
          />
          <MenuItem icon="new-object" text="New object" />
          <MenuItem icon="new-link" text="New link" />
          <Divider />
          <MenuItem icon="log-out" text="Sign out" href="/accounts/sign-out" />
        </Menu>
      }
    >
      <Button icon="user" text="My profile" />
    </Popover>
  );
};

export default Profile;
