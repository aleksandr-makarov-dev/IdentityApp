import { JwtPayload } from "jwt-decode";
import { z } from "zod";

export const signInLocalSchema = z.object({
  email: z.string().email().min(1),
  password: z.string().min(6),
});

export type SignInLocalSchema = z.infer<typeof signInLocalSchema>;

export type ExternalProviderScheme = {
  name: string;
  displayName?: string;
};

export type JwtResponse = {
  token: string;
};

export interface Profile extends JwtPayload {
  email: string;
  nameid: string;
  role: string;
}
