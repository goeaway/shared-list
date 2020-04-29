import { ListDTO } from "../../src/types";
import createListItem from "./create-list-item";

export default function createList() : ListDTO {
    return {
        id: "id",
        name: "list",
        items: [createListItem()]
    }
}