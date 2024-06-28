import {
  AnchorButton,
  Classes,
  Colors,
  Navbar,
  NavbarDivider,
} from "@blueprintjs/core";
import React, { FC } from "react";
import { Outlet } from "react-router-dom";
import Profile from "../features/accounts/components/Profile";
import { useSession } from "../features/accounts/providers/SessionProvider";

const MainLayout: FC = () => {
  const { isAuthenticated } = useSession();

  return (
    <div
      className="min-h-screen"
      style={{ backgroundColor: Colors.LIGHT_GRAY5 }}
    >
      <Navbar>
        <Navbar.Group>
          <Navbar.Heading className="font-medium">IdentityApp</Navbar.Heading>
          <NavbarDivider />
          <AnchorButton
            className={Classes.MINIMAL}
            icon="home"
            text="Home"
            href="/"
          />
        </Navbar.Group>
        <Navbar.Group align="right" className="space-x-3">
          {isAuthenticated ? (
            <Profile />
          ) : (
            <React.Fragment>
              <AnchorButton text="Register" href="/accounts/register" />
              <AnchorButton
                intent="primary"
                text="Sign in"
                href="/accounts/sign-in"
              />
            </React.Fragment>
          )}
        </Navbar.Group>
      </Navbar>
      <div className="py-12 max-w-screen-lg mx-auto w-full">
        <Outlet />
      </div>
    </div>
  );
};

export default MainLayout;
