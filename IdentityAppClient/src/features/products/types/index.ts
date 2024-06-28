import { z } from "zod";

export type Product = {
  id: number;
  name: string;
  price: number;
  category: string;
};

export const editProductSchema = z.object({
  name: z.string().min(1),
  price: z.number(),
  category: z.string().min(1),
});

export type EditProductSchema = z.infer<typeof editProductSchema>;
