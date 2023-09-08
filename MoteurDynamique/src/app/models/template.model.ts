import { Group } from "./group.model";

export interface Template {
    id:number;
    name: string;
    state: string;
    style: string;
    language:string;
    IdDoc:string;
    groups: Group[];
  }