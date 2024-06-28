import { Alert, OverlayToaster } from "@blueprintjs/core";
import { FC } from "react";
import { Product } from "../types";
import { useDeleteProduct } from "../api/deleteProduct";
import { useQueryClient } from "@tanstack/react-query";

interface DeleteProductDialogProps {
  product: Product;
  open: boolean;
  setOpen: React.Dispatch<React.SetStateAction<boolean>>;
}

const DeleteProductDialog: FC<DeleteProductDialogProps> = ({
  product,
  open,
  setOpen,
}) => {
  const queryClient = useQueryClient();

  const { mutate, isPending } = useDeleteProduct();

  const handleClose = () => {
    setOpen(false);
  };

  const handleDelete = () => {
    mutate(product.id, {
      onSuccess: () => {
        queryClient.setQueryData<Product[]>(["products"], (prev) => {
          return prev?.filter((p) => p.id != product.id);
        });

        handleClose();
      },
    });
  };

  return (
    <Alert
      intent="danger"
      isOpen={open}
      onCancel={handleClose}
      onConfirm={handleDelete}
      cancelButtonText="Cancel"
      confirmButtonText="Move to Trash"
      loading={isPending}
    >
      <p>
        Are you sure you want to remove product <b>{product.name}</b>? You will
        not be able to restore it later.
      </p>
    </Alert>
  );
};

export default DeleteProductDialog;
