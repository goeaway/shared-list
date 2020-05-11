import { AuthenticationData } from "../context/auth";

const AUTH_DATA_KEY = "sharedlist_auth";

export const getAuthData = () : AuthenticationData => {
    try {
        const str = localStorage.getItem(AUTH_DATA_KEY);
        const data = JSON.parse(str) as AuthenticationData;

        // if expiry date is falsy or is older than now
        // remove the item and return undefined
        if(!data.expires || data.expires < new Date().getTime()) {
            clearAuthData();
            return undefined;
        }

        return data;
    } catch {
        return undefined;
    }
}

export const storeAuthData = (data: AuthenticationData) => {
    if(!data) {
        clearAuthData();
    }

    const str = JSON.stringify(data);

    try {
        localStorage.setItem(AUTH_DATA_KEY, str);
    } catch {

    }
}

export const clearAuthData = () => {
    try {
        localStorage.removeItem(AUTH_DATA_KEY);
    } catch {

    }
}