import { UseMutationOptions, useMutation } from "@tanstack/react-query";
import axios from "../../../lib/axios";

const deleteProduct = async (id: number) => {
  await axios.delete(`/products/${id}`);
};

type UseDeleteProductMutation = UseMutationOptions<
  void,
  Error,
  number,
  unknown[]
>;

type UseDeleteProductOptions = Omit<
  UseDeleteProductMutation,
  "mutationKey" | "mutationFn"
>;

export const useDeleteProduct = (options?: UseDeleteProductOptions) => {
  return useMutation({
    mutationKey: ["products", "delete"],
    mutationFn: async (id) => {
      return await deleteProduct(id);
    },
    ...options,
  });
};
