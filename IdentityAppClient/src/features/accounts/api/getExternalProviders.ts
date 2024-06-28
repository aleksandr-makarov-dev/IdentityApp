import { UseQueryOptions, useQuery } from "@tanstack/react-query";
import { ExternalProviderScheme } from "../types";
import axios from "../../../lib/axios";
import { AxiosError } from "axios";
import { ProblemDetails } from "../../../types";

const getExternalProviders = async () => {
  const response = await axios.get<ExternalProviderScheme[]>(
    "/accounts/sign-in/external/providers"
  );

  return response.data;
};

type UseExternalProvidersQuery = UseQueryOptions<
  ExternalProviderScheme[],
  AxiosError<ProblemDetails>,
  ExternalProviderScheme[],
  unknown[]
>;

type UseExternalProvidersOptions = Omit<
  UseExternalProvidersQuery,
  "queryKey" | "queryFn"
>;

export const useExternalProviders = (options?: UseExternalProvidersOptions) => {
  return useQuery({
    queryKey: ["accounts", "sign-in", "external-providers"],
    queryFn: async () => {
      return getExternalProviders();
    },
    ...options,
  });
};
