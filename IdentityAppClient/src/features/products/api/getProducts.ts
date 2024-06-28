import { UseQueryOptions, useQuery } from "@tanstack/react-query";
import { Product } from "../types";
import axios from "../../../lib/axios";

const getProducts = async () => {
  const response = await axios.get<Product[]>("/products");
  return response.data;
};

type UseProductsQuery = UseQueryOptions<Product[], Error, Product[], unknown[]>;

type UseProductsOptions = Omit<UseProductsQuery, "queryKey" | "queryFn">;

export const useProducts = (options?: UseProductsOptions) => {
  return useQuery({
    queryKey: ["products"],
    queryFn: async () => {
      return getProducts();
    },
    ...options,
  });
};
