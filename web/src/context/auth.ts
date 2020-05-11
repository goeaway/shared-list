import { createContext } from "react";

export interface Profile {
    email: string;
    name: string;
    image: string;
}

export interface AuthenticationData {
    jwt: string;
    profile: Profile;
    expires: number;
}

const AuthContext = createContext<{ authData: AuthenticationData, isAuthed: (authData: AuthenticationData) => boolean, setAuthentication: (authData: AuthenticationData) => void}>(undefined);

export default AuthContext;