import {
  Button,
  Callout,
  Dialog,
  DialogBody,
  DialogFooter,
  OverlayToaster,
} from "@blueprintjs/core";
import React from "react";
import { FC, useState } from "react";
import ProductForm from "./ProductForm";
import { EditProductSchema, Product } from "../types";
import { useCreateProduct } from "../api/createProduct";
import { useQueryClient } from "@tanstack/react-query";

interface CreateProductDialogProps {
  trigger: JSX.Element;
}

const CreateProductDialog: FC<CreateProductDialogProps> = ({ trigger }) => {
  const queryClient = useQueryClient();
  const { mutate, isPending, isError, error } = useCreateProduct();

  const [open, setOpen] = useState<boolean>(false);

  const onOpen = () => {
    setOpen(true);
  };

  const onClose = () => {
    setOpen(false);
  };

  const onSubmit = (values: EditProductSchema) => {
    mutate(values, {
      onSuccess: async (newProduct) => {
        queryClient.setQueryData<Product[]>(["products"], (prev) => {
          return prev ? [...prev, newProduct] : [newProduct];
        });

        const toast = await OverlayToaster.createAsync({
          position: "top",
        });

        toast.show({
          message: "New product created",
          intent: "success",
        });

        onClose();
      },
    });
  };

  return (
    <React.Fragment>
      {React.cloneElement(trigger, { onClick: onOpen })}
      <Dialog
        title="Create product"
        icon="cube-add"
        isOpen={open}
        onClose={onClose}
        canOutsideClickClose={false}
      >
        <DialogBody className="space-y-3">
          {isError && (
            <Callout intent="danger" title="An error occured">
              {error.message}
            </Callout>
          )}
          <ProductForm id="createProductForm" onSubmit={onSubmit} />
        </DialogBody>
        <DialogFooter
          actions={
            <React.Fragment>
              <Button onClick={onClose} disabled={isPending}>
                Cancel
              </Button>
              <Button
                intent="primary"
                type="submit"
                form="createProductForm"
                loading={isPending}
              >
                Create
              </Button>
            </React.Fragment>
          }
        />
      </Dialog>
    </React.Fragment>
  );
};

export default CreateProductDialog;
