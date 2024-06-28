import { FC } from "react";
import { Controller, useForm } from "react-hook-form";
import { SignInLocalSchema, signInLocalSchema } from "../types";
import { zodResolver } from "@hookform/resolvers/zod";
import { AnchorButton, Button, FormGroup, InputGroup } from "@blueprintjs/core";
import React from "react";

interface SignInFormProps {
  onSubmit: (values: SignInLocalSchema) => void;
  isLoading?: boolean;
}

const SignInForm: FC<SignInFormProps> = ({ onSubmit, isLoading }) => {
  const form = useForm<SignInLocalSchema>({
    resolver: zodResolver(signInLocalSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  });

  return (
    <React.Fragment>
      <form onSubmit={form.handleSubmit(onSubmit)}>
        <Controller
          control={form.control}
          name="email"
          render={({
            field: { ref, ...other },
            fieldState: { invalid, error },
          }) => (
            <FormGroup
              label="Email"
              intent={invalid ? "danger" : "none"}
              helperText={invalid && error?.message}
            >
              <InputGroup
                type="email"
                inputRef={ref}
                intent={invalid ? "danger" : "none"}
                {...other}
              />
            </FormGroup>
          )}
        />
        <Controller
          control={form.control}
          name="password"
          render={({
            field: { ref, ...other },
            fieldState: { invalid, error },
          }) => (
            <FormGroup
              label="Password"
              intent={invalid ? "danger" : "none"}
              helperText={invalid && error?.message}
            >
              <InputGroup
                type="password"
                inputRef={ref}
                intent={invalid ? "danger" : "none"}
                {...other}
              />
            </FormGroup>
          )}
        />
        <div className="space-x-3">
          <Button type="submit" intent="primary" loading={isLoading}>
            Sign In
          </Button>
          <AnchorButton href="/accouns/reset-password" disabled={isLoading}>
            Forgot password?
          </AnchorButton>
        </div>
      </form>
    </React.Fragment>
  );
};

export default SignInForm;
