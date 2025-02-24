import { Funcionario } from "./Funcionario";

export interface Projeto {
    id: number;
    nome: string;
    responsavelId: number;
    responsavel: Funcionario;
    subResponsavelId: number;
    subResponsavel: Funcionario;
    resultado?: string;
    entrega1?: string;
    entrega2?: string;
    entrega3?: string;
    nota?: number;
}
