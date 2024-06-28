import { UseMutationOptions, useMutation } from "@tanstack/react-query";
import { JwtResponse } from "../types";
import axios from "../../../lib/axios";
import { AxiosError } from "axios";
import { ProblemDetails } from "../../../types";

const refreshToken = async () => {
  const response = await axios.post<JwtResponse>("/accounts/refresh-token");
  return response.data;
};

type UseRefreshTokenMutation = UseMutationOptions<
  JwtResponse,
  AxiosError<ProblemDetails>,
  unknown,
  unknown[]
>;

type UseRefreshTokenOptions = Omit<
  UseRefreshTokenMutation,
  "mutationKey" | "mutationFn"
>;

export const useRefreshToken = (options?: UseRefreshTokenOptions) => {
  return useMutation({
    mutationKey: ["accounts", "refresh-token"],
    mutationFn: async () => {
      return await refreshToken();
    },
    ...options,
  });
};
