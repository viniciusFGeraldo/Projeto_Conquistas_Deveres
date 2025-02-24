import { EscalaMes } from "./EscalaMes";
import { EscalaRobo } from "./EscalaRobo";
import { Projeto } from "./Projeto";

export interface Funcionario {
    id: number;
    nome: string;
    fotoCaminho?: string;
    projetosComoResponsavel: Projeto[];
    projetosComoSubResponsavel: Projeto[];
    escalasRobo: EscalaRobo[];
    escalasMes: EscalaMes[];
}
