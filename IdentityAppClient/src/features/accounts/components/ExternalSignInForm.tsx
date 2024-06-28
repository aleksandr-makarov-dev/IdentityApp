import { FC } from "react";
import { useExternalProviders } from "../api/getExternalProviders";
import { AnchorButton, H6 } from "@blueprintjs/core";
import React from "react";

const ExternalSignInForm: FC = () => {
  const { data, isLoading, isError } = useExternalProviders();

  if (isLoading || isError) {
    return null;
  }

  return (
    <React.Fragment>
      <H6 className="text-center">Or Sign In with External Provider</H6>
      <form className="grid grid-cols-2 gap-3">
        {data?.map((p) => (
          <AnchorButton
            key={p.name}
            text={p.displayName ?? p.name}
            href={`${import.meta.env.VITE_API_URL}/accounts/sign-in/external/${
              p.name
            }?returnUrl=${encodeURI(import.meta.env.VITE_URL)}`}
          />
        ))}
      </form>
    </React.Fragment>
  );
};

export default ExternalSignInForm;
