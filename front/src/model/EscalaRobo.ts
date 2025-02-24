import { Funcionario } from "./Funcionario";

export interface EscalaRobo {
    id: number;
    title: string; // Sempre será "Escala do Robô"
    mesAtual: string; // Formato MM/yyyy
    funcionarioId: number;
    funcionario?: Funcionario;
    datas: string[]; // Convertendo DateTime para string no formato yyyy-MM-dd
}
