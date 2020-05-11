import { AuthenticationData } from "../context/auth";

export const isAuthenticated = (data: AuthenticationData) => {
    return data && !!data.jwt && data.expires && data.expires > new Date().getTime();
}