import { ListDTO } from "../types";
import { v1 } from "uuid";

export const createNewList = () : ListDTO => ({
    id: v1(),
    name: "New List",
    items: []
});