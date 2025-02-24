import React, { useState } from 'react';

interface Projeto {
  nome: string;
  responsavel: string;
  subResponsavel: string;
  resultado: string;
  entrega1: string;
  entrega2: string;
  entrega3: string;
  nota: number;
}

const FormularioProjeto: React.FC = () => {
  const [projeto, setProjeto] = useState<Projeto>({
    nome: '',
    responsavel: '',
    subResponsavel: '',
    resultado: '',
    entrega1: '',
    entrega2: '',
    entrega3: '',
    nota: 0,
  });

  // Função para atualizar o estado conforme o usuário preenche os dados
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setProjeto((prevProjeto) => ({
      ...prevProjeto,
      [name]: value,
    }));
  };

  return (
    <div>
      <h1>Formulário de Projeto</h1>

      {/* Formulário de entrada de dados */}
      <form>
        <div>
          <label htmlFor="nome">Nome do Projeto:</label>
          <input
            type="text"
            id="nome"
            name="nome"
            value={projeto.nome}
            onChange={handleChange}
          />
        </div>

        <div>
          <label htmlFor="responsavel">Responsável:</label>
          <input
            type="text"
            id="responsavel"
            name="responsavel"
            value={projeto.responsavel}
            onChange={handleChange}
          />
        </div>

        <div>
          <label htmlFor="subResponsavel">Sub-Responsável:</label>
          <input
            type="text"
            id="subResponsavel"
            name="subResponsavel"
            value={projeto.subResponsavel}
            onChange={handleChange}
          />
        </div>

        <div>
          <label htmlFor="resultado">Resultado:</label>
          <input
            type="text"
            id="resultado"
            name="resultado"
            value={projeto.resultado}
            onChange={handleChange}
          />
        </div>

        <div>
          <label htmlFor="entrega1">Entrega 1:</label>
          <input
            type="text"
            id="entrega1"
            name="entrega1"
            value={projeto.entrega1}
            onChange={handleChange}
          />
        </div>

        <div>
          <label htmlFor="entrega2">Entrega 2:</label>
          <input
            type="text"
            id="entrega2"
            name="entrega2"
            value={projeto.entrega2}
            onChange={handleChange}
          />
        </div>

        <div>
          <label htmlFor="entrega3">Entrega 3:</label>
          <input
            type="text"
            id="entrega3"
            name="entrega3"
            value={projeto.entrega3}
            onChange={handleChange}
          />
        </div>

        <div>
          <label htmlFor="nota">Nota:</label>
          <input
            type="number"
            id="nota"
            name="nota"
            value={projeto.nota}
            onChange={handleChange}
          />
        </div>
      </form>

      {/* Visualização em tempo real do projeto */}
      <div style={{ marginTop: '20px', padding: '20px', border: '1px solid #ccc' }}>
        <h2>Modelo do Projeto</h2>
        <p><strong>Nome:</strong> {projeto.nome}</p>
        <p><strong>Responsável:</strong> {projeto.responsavel}</p>
        <p><strong>Sub-Responsável:</strong> {projeto.subResponsavel}</p>
        <p><strong>Resultado:</strong> {projeto.resultado}</p>
        <p><strong>Entrega 1:</strong> {projeto.entrega1}</p>
        <p><strong>Entrega 2:</strong> {projeto.entrega2}</p>
        <p><strong>Entrega 3:</strong> {projeto.entrega3}</p>
        <p><strong>Nota:</strong> {projeto.nota}</p>
      </div>
    </div>
  );
};

export default FormularioProjeto;
