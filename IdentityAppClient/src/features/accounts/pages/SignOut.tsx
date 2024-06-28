import {
  AnchorButton,
  Button,
  Callout,
  H4,
  Section,
  SectionCard,
} from "@blueprintjs/core";
import { FC } from "react";
import { useSession } from "../providers/SessionProvider";
import { useSignOut } from "../api/signOut";
import { useNavigate } from "react-router-dom";

const SignOut: FC = () => {
  const { profile, signOut } = useSession();
  const navigate = useNavigate();
  const { mutate, isPending, isError, error } = useSignOut();

  const handleSignOut = () => {
    mutate(undefined, {
      onSuccess: () => {
        signOut();
        navigate("/");
      },
    });
  };

  return (
    <div className="space-y-5">
      {isError && (
        <Callout intent="danger" title="An error occured">
          {error.message}
        </Callout>
      )}
      <Section
        className="mx-auto max-w-md"
        title="Sign out of account"
        titleRenderer={H4}
      >
        <SectionCard>
          <p>
            Are you sure you want to sign out from <b>{profile?.email}</b> ?
          </p>
          <div className="flex gap-3 justify-end">
            <AnchorButton href="/" disabled={isPending}>
              Cancel
            </AnchorButton>
            <Button intent="danger" loading={isPending} onClick={handleSignOut}>
              Sign out
            </Button>
          </div>
        </SectionCard>
      </Section>
    </div>
  );
};

export default SignOut;
