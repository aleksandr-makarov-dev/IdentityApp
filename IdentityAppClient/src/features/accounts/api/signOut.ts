import { UseMutationOptions, useMutation } from "@tanstack/react-query";
import axios from "../../../lib/axios";
import { AxiosError } from "axios";
import { ProblemDetails } from "../../../types";

const signOut = async () => {
  const response = await axios.delete("/accounts/sign-out");
  return response.data;
};

type UseSignOutMutation = UseMutationOptions<
  unknown,
  AxiosError<ProblemDetails>,
  unknown,
  unknown[]
>;

type UseSignOutOptions = Omit<UseSignOutMutation, "mutationKey" | "mutationFn">;

export const useSignOut = (options?: UseSignOutOptions) => {
  return useMutation({
    mutationKey: ["accounts", "sign-out"],
    mutationFn: async () => {
      return await signOut();
    },
    ...options,
  });
};
