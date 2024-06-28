import { FormGroup, InputGroup, NumericInput } from "@blueprintjs/core";
import { FC, HTMLAttributes } from "react";
import { Controller, useForm } from "react-hook-form";
import { EditProductSchema, editProductSchema } from "../types";
import { zodResolver } from "@hookform/resolvers/zod";

interface ProductFormProps
  extends Omit<HTMLAttributes<HTMLFormElement>, "onSubmit"> {
  onSubmit: (values: EditProductSchema) => void;
}

const ProductForm: FC<ProductFormProps> = ({ onSubmit, ...other }) => {
  const form = useForm<EditProductSchema>({
    resolver: zodResolver(editProductSchema),
    defaultValues: {
      name: "",
      price: 0,
      category: "",
    },
  });

  return (
    <form onSubmit={form.handleSubmit(onSubmit)} {...other}>
      <Controller
        control={form.control}
        name="name"
        render={({
          field: { ref, ...other },
          fieldState: { invalid, error },
        }) => (
          <FormGroup
            label="Name"
            intent={invalid ? "danger" : "none"}
            helperText={invalid && error?.message}
          >
            <InputGroup
              intent={invalid ? "danger" : "none"}
              inputRef={ref}
              {...other}
            />
          </FormGroup>
        )}
      />
      <Controller
        control={form.control}
        name="price"
        render={({
          field: { ref, onChange, ...other },
          fieldState: { invalid, error },
        }) => (
          <FormGroup
            label="Price"
            intent={invalid ? "danger" : "none"}
            helperText={invalid && error?.message}
          >
            <NumericInput
              fill
              type="number"
              intent={invalid ? "danger" : "none"}
              buttonPosition="none"
              inputRef={ref}
              onValueChange={(e) => onChange(e)}
              {...other}
            />
          </FormGroup>
        )}
      />
      <Controller
        control={form.control}
        name="category"
        render={({
          field: { ref, ...other },
          fieldState: { invalid, error },
        }) => (
          <FormGroup
            label="Category"
            intent={invalid ? "danger" : "none"}
            helperText={invalid && error?.message}
          >
            <InputGroup
              inputRef={ref}
              intent={invalid ? "danger" : "none"}
              {...other}
            />
          </FormGroup>
        )}
      />
    </form>
  );
};

export default ProductForm;
