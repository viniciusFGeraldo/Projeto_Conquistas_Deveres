export interface EscalaMes {
    id: number;
    mes: string; // Formato MM/yyyy
    datasDisponiveis: string[]; // Convertendo DateTime para string no formato yyyy-MM-dd
}
