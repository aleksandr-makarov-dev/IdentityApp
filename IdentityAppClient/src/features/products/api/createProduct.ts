import { UseMutationOptions, useMutation } from "@tanstack/react-query";
import { EditProductSchema, Product } from "../types";
import axios from "../../../lib/axios";
import { AxiosError } from "axios";
import { ProblemDetails } from "../../../types";

const createProduct = async (values: EditProductSchema) => {
  const response = await axios.post<Product>("/products", values);

  return response.data;
};

type UseCreateProductMutation = UseMutationOptions<
  Product,
  AxiosError<ProblemDetails>,
  EditProductSchema,
  unknown[]
>;

type UseCreateProductOptions = Omit<
  UseCreateProductMutation,
  "mutationKey" | "mutationFn"
>;

export const useCreateProduct = (options?: UseCreateProductOptions) => {
  return useMutation({
    mutationKey: ["products", "create"],
    mutationFn: async (values) => {
      return await createProduct(values);
    },
    ...options,
  });
};
