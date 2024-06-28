import {
  Button,
  Callout,
  Classes,
  HTMLTable,
  Section,
} from "@blueprintjs/core";
import { FC, useState } from "react";
import { useProducts } from "../api/getProducts";
import CreateProductDialog from "./CreateProductDialog";
import DeleteProductDialog from "./DeleteProductDialog";
import { Product } from "../types";

const ProductSkeleton: FC = () => {
  return (
    <tr>
      <td>
        <p className={Classes.SKELETON}>i</p>
      </td>
      <td>
        <p className={Classes.SKELETON}>name</p>
      </td>
      <td>
        <p className={Classes.SKELETON}>$price</p>
      </td>
      <td>
        <p className={Classes.SKELETON}>category</p>
      </td>
      <td className="flex justify-end">
        <Button className={Classes.SKELETON} icon="trash" />
      </td>
    </tr>
  );
};

const ProductList: FC = () => {
  const { data, isLoading, isError, error } = useProducts();

  const [deleteDialogOpen, setDeleteDialogOpen] = useState<boolean>(false);
  const [selected, setSelected] = useState<Product | null>(null);

  const onDeleteProduct = (product: Product) => {
    setSelected(product);

    setDeleteDialogOpen(true);
  };

  return (
    <div className="space-y-5">
      {isError && (
        <Callout intent="danger" title="An error occured">
          {error.message}
        </Callout>
      )}
      <Section
        title="Products"
        subtitle="Available to purchase in the shop"
        rightElement={
          <CreateProductDialog
            trigger={
              <Button
                intent="primary"
                icon="cube-add"
                text="Create product"
                disabled={isLoading || isError}
              />
            }
          />
        }
      >
        <HTMLTable className="w-full" bordered striped>
          <thead>
            <tr>
              <th>Id</th>
              <th>Name</th>
              <th>Price</th>
              <th>Category</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {isLoading &&
              Array(8)
                .fill(0)
                .map((_, i) => <ProductSkeleton key={i} />)}

            {data?.map((product) => (
              <tr key={product.id}>
                <td className="whitespace-nowrap">{product.id}</td>
                <td className="whitespace-nowrap">{product.name}</td>
                <td className="whitespace-nowrap">${product.price}</td>
                <td className="whitespace-nowrap">{product.category}</td>
                <td className="whitespace-nowrap flex justify-end gap-3">
                  <Button
                    intent="danger"
                    icon="trash"
                    onClick={() => onDeleteProduct(product)}
                  />
                </td>
              </tr>
            ))}
          </tbody>
        </HTMLTable>
      </Section>
      {selected && (
        <DeleteProductDialog
          open={deleteDialogOpen}
          setOpen={setDeleteDialogOpen}
          product={selected}
        />
      )}
    </div>
  );
};

export default ProductList;
