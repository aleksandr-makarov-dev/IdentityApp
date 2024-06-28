import { UseMutationOptions, useMutation } from "@tanstack/react-query";
import { JwtResponse, SignInLocalSchema } from "../types";
import axios from "../../../lib/axios";
import { AxiosError } from "axios";
import { ProblemDetails } from "../../../types";

const signInLocal = async (values: SignInLocalSchema) => {
  const response = await axios.post<JwtResponse>(
    "/accounts/sign-in/local",
    values
  );
  return response.data;
};

type UseSignInLocalMutation = UseMutationOptions<
  JwtResponse,
  AxiosError<ProblemDetails>,
  SignInLocalSchema,
  unknown[]
>;

type UseSignInLocalOptions = Omit<
  UseSignInLocalMutation,
  "mutationKey" | "mutationFn"
>;

export const useSignInLocal = (options?: UseSignInLocalOptions) => {
  return useMutation({
    mutationKey: ["accounts", "sign-in", "local"],
    mutationFn: async (values) => {
      return await signInLocal(values);
    },
    ...options,
  });
};
