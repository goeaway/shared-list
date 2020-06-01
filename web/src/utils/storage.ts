import { AuthenticationData } from "../context/auth";
import { ListPreviewDTO } from "@src/types";

const AUTH_DATA_KEY = "sharedlist_auth";
const LISTS_KEY = "sharedlist_lists";

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

export const getListPreviews = () : Array<ListPreviewDTO> => {
    try {
        const str = localStorage.getItem(LISTS_KEY);

        if(!str) {
            return [];
        }

        return JSON.parse(str) as Array<ListPreviewDTO>;
    } catch {
        return [];
    }
}

export const setListPreviews = (lists: Array<ListPreviewDTO>) => {
    try {
        localStorage.setItem(LISTS_KEY, JSON.stringify(lists));
    } catch {

    }
}