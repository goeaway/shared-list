import { ListDTO } from "../types";

export const createNewList = () : ListDTO => ({
    id: undefined,
    name: "New List",
    items: []
});