export interface ListDTO {
    id: string;
    items: Array<ListItemDTO>,
    name: string;
}

export interface ListItemDTO {
    value: string;
    notes: string;
    id: string;
    completed: boolean;
}

export interface NameAndId<TId> {
    name: string;
    id: TId;
}

export interface AuthenticationResponse {
    jwt: string;
    email: string;
    name: string;
}