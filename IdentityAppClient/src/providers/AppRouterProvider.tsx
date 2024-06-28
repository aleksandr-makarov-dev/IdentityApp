import { RouterProvider, createBrowserRouter } from "react-router-dom";
import MainLayout from "../layouts/MainLayout";
import Products from "../features/products/pages/Products";
import { FC } from "react";
import SignIn from "../features/accounts/pages/SignIn";
import SignOut from "../features/accounts/pages/SignOut";

const router = createBrowserRouter([
  {
    path: "/",
    children: [
      {
        element: <MainLayout />,
        children: [
          {
            index: true,
            element: <Products />,
          },
          {
            path: "accounts",
            children: [
              {
                path: "sign-in",
                element: <SignIn />,
              },
              {
                path: "sign-out",
                element: <SignOut />,
              },
            ],
          },
        ],
      },
    ],
  },
]);

export const AppRouterProvider: FC = () => {
  return <RouterProvider router={router} />;
};
