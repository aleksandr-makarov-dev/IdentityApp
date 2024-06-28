import { FC } from "react";
import SignInForm from "../components/SignInForm";
import { Callout, H3, Section, SectionCard } from "@blueprintjs/core";
import ExternalSignInForm from "../components/ExternalSignInForm";
import { useSignInLocal } from "../api/signInLocal";
import { useNavigate } from "react-router-dom";
import { SignInLocalSchema } from "../types";
import { useSession } from "../providers/SessionProvider";

const SignIn: FC = () => {
  const { signIn } = useSession();
  const navigate = useNavigate();
  const { mutate, isPending, isError, error } = useSignInLocal();

  const handleSignInLocal = (values: SignInLocalSchema) => {
    mutate(values, {
      onSuccess: ({ token }) => {
        signIn(token);
        navigate("/");
      },
    });
  };

  return (
    <div>
      <Section
        icon="key"
        title="Sign in"
        titleRenderer={H3}
        className="mx-auto max-w-md"
      >
        <SectionCard className="space-y-5">
          {isError && (
            <Callout intent="danger" title="An error occured">
              {error.message}
            </Callout>
          )}
          <SignInForm onSubmit={handleSignInLocal} isLoading={isPending} />
          <ExternalSignInForm />
        </SectionCard>
      </Section>
    </div>
  );
};

export default SignIn;
