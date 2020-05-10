import { createContext } from "react";

export interface Profile {
    email: string;
    name: string;
}

export interface AuthenticationData {
    jwt: string;
    profile: Profile;
}

const AuthContext = createContext<{ authData: AuthenticationData, isAuthed: boolean, setAuthentication: (authData: AuthenticationData) => void}>(undefined);

export default AuthContext;