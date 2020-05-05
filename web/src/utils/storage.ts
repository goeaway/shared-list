
const KEY = "shared_lists";

export function getLists() : Array<string> {
    try {
        return JSON.parse(localStorage.getItem(KEY)) as Array<string> || [];
    }
    catch {
        return [];
    }
}

export function addListIfNew(listId: string) {
    const existing = getLists() || [];

    if(existing.indexOf(listId) > -1) {
        return;
    }

    const updated = [...existing, listId];

    try {
        localStorage.setItem(KEY, JSON.stringify(updated));
    } catch {
        // ignore
    }
}

export function addList(listId: string) {
    const existing = getLists() || [];
    const updated = [...existing, listId];

    try {
        localStorage.setItem(KEY, JSON.stringify(updated));
    } catch {
        // ignore
    }
}