import React, { useState, useEffect } from "react";
import axios from "axios";
import { Funcionario } from "../model/Funcionario";
import FOTO_DEFAULT from "./fotodefault.png";

const API_URL = "http://localhost:5003/funcionarios";

export default function Funcionarios() {
  const [funcionarios, setFuncionarios] = useState<Funcionario[]>([]);
  const [nome, setNome] = useState("");
  const [foto, setFoto] = useState<File | null>(null);
  const [editandoId, setEditandoId] = useState<number | null>(null);
  const [fotoRedimensionada, setFotoRedimensionada] = useState<string | null>(null);

  useEffect(() => {
    carregarFuncionarios();
  }, []);

  const carregarFuncionarios = async () => {
    try {
      const response = await axios.get(API_URL);
      setFuncionarios(response.data);
    } catch (error) {
      console.error("Erro ao buscar funcionários", error);
    }
  };

  const redimensionarFoto = (file: File) => {
    const reader = new FileReader();
    reader.onload = (e) => {
      if (e.target && e.target.result) {
        const img = new Image();
        img.onload = () => {
          const canvas = document.createElement("canvas");
          const ctx = canvas.getContext("2d");
          if (ctx) {
            const largura = img.width;
            const altura = img.height;
            canvas.width = largura;
            canvas.height = altura;

            ctx.imageSmoothingEnabled = true;
            ctx.imageSmoothingQuality = "high";
            ctx.drawImage(img, 0, 0, largura, altura);
            setFotoRedimensionada(canvas.toDataURL("image/jpeg", 0.92));
          }
        };
        img.src = e.target.result as string;
      }
    };
    reader.readAsDataURL(file);
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (!nome.trim()) {
      alert("O nome do funcionário é obrigatório.");
      return;
    }

    const formData = new FormData();
    formData.append("nome", nome);

    // Se a foto foi redimensionada, usa ela
    let fotoBlob: Blob | null = null;
    if (fotoRedimensionada) {
      const base64Data = fotoRedimensionada.split(",")[1];
      const byteString = atob(base64Data);
      const uintArray = new Uint8Array(byteString.length);
      for (let i = 0; i < byteString.length; i++) {
        uintArray[i] = byteString.charCodeAt(i);
      }
      fotoBlob = new Blob([uintArray], { type: "image/jpeg" });
    } else if (foto) {
      fotoBlob = foto;
    } else if (FOTO_DEFAULT) {
      const response = await fetch(FOTO_DEFAULT);
      const blob = await response.blob();
      fotoBlob = blob;
    }

    if (fotoBlob) {
      formData.append("foto", fotoBlob, "foto.jpg");
    }

    try {
      if (editandoId) {
        await axios.put(`${API_URL}/${editandoId}`, formData);
      } else {
        await axios.post(API_URL, formData);
      }

      // Limpar o formulário
      setNome("");
      setFoto(null);
      setEditandoId(null);
      setFotoRedimensionada(null);

      // Atualizar a lista de funcionários
      carregarFuncionarios();
    } catch (error) {
      console.error("Erro ao salvar funcionário:", error);
      alert("Erro ao salvar funcionário. Tente novamente.");
    }
  };

  const handleDelete = async (id: number) => {
    try {
      await axios.delete(`${API_URL}/${id}`);
      carregarFuncionarios();
    } catch (error) {
      console.error("Erro ao deletar funcionário", error);
    }
  };

  const handleEdit = (funcionario: Funcionario) => {
    setNome(funcionario.nome);
    setEditandoId(funcionario.id);

    // Carregar a foto se existir
    if (funcionario.fotoCaminho) {
      setFotoRedimensionada(`http://localhost:5003/${funcionario.fotoCaminho}`);
    } else {
      setFotoRedimensionada(null); // Ou FOTO_DEFAULT caso deseje manter uma foto padrão
    }
  };


  return (
    <div className="p-6">
      <div className="row">
        <div className="col-md-4" style={{ margin: "2vw 0vw 0vw 2vw" }}>
          <div className="card border-warning mb-4 shadow-lg">
            <div className="card-body">
              <h5 className="card-title text-center mb-4">
                {editandoId ? "Alterar Funcionário" : "Cadastrar Funcionário"}
              </h5>
              <form onSubmit={handleSubmit} className="d-flex flex-column gap-3">
                <div className="form-group" style={{ display: "flex", justifyContent: "center", alignItems: "center", flexDirection: "column" }}>
                  <label
                    htmlFor="foto"
                    style={{
                      width: "70%",
                      height: "calc(100vw * 0.3)",
                      backgroundColor: "#f0f0f0",
                      display: "flex",
                      justifyContent: "center",
                      alignItems: "center",
                      border: "2px solid #ccc",
                      borderRadius: "8px",
                      cursor: "pointer",
                      textAlign: "center",
                      backgroundImage: fotoRedimensionada ? `url(${fotoRedimensionada})` : "none",
                      backgroundSize: "cover",
                      backgroundPosition: "center",
                      position: "relative",
                    }}
                  >
                    {!fotoRedimensionada && "Selecionar foto"}
                    <input
                      type="file"
                      id="foto"
                      onChange={(e) => {
                        const file = e.target.files ? e.target.files[0] : null;
                        if (file) {
                          setFoto(file);
                          redimensionarFoto(file);
                        }
                      }}
                      className="form-control border p-2 rounded"
                      style={{
                        width: "100%",
                        height: "100%",
                        opacity: 0,
                        position: "absolute",
                        cursor: "pointer",
                      }}
                    />
                  </label>

                </div>

                <div className="form-group" style={{ display: "flex", justifyContent: "center", alignItems: "center" }}>
                  <input
                    type="text"
                    placeholder="Nome"
                    value={nome}
                    onChange={(e) => setNome(e.target.value)}
                    required
                    className="form-control border p-2 rounded"
                    style={{
                      width: "80%",
                      maxWidth: "60vw",
                    }}
                  />
                </div>

                <div className="form-group" style={{ display: "flex", justifyContent: "center", alignItems: "center" }}>
                  <button
                    type="submit"
                    className="btn btn-primary py-2 mt-3"
                    style={{
                      width: "70%",
                      maxWidth: "40vw",
                      backgroundColor: "green",
                      borderColor: "green",
                    }}
                  >
                    {editandoId ? "Atualizar" : "Cadastrar"}
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>

        <div className="col-md-7" style={{ margin: "2vw 0 0 2vw" }}>
          <div className="row row-cols-1 row-cols-md-3 g-2">
            {funcionarios.map((funcionario) => (
              <div key={funcionario.id} className="col">
                <div className="card border-warning rounded shadow-lg">
                  <div className="card-body">
                    <img
                      src={funcionario.fotoCaminho ? `http://localhost:5003/${funcionario.fotoCaminho}` : FOTO_DEFAULT}
                      alt={funcionario.nome}
                      className="img-fluid"
                      style={{
                        width: "80%",
                        height: "140px",  // Tamanho fixo
                        objectFit: "contain", // Ajusta a imagem para manter a proporção, com fundo branco onde necessário
                        display: "block",
                        marginLeft: "auto",
                        marginRight: "auto",
                        backgroundColor: "white", // Preenchimento branco
                      }}
                    />

                    <h5 className="card-title text-center mt-3 fw-light">{funcionario.nome}</h5>

                    <div className="d-flex justify-content-center mt-2">
                      <button
                        onClick={() => handleEdit(funcionario)}
                        className="btn btn-warning btn-sm me-2"
                      >
                        Editar
                      </button>
                      <button
                        onClick={() => handleDelete(funcionario.id)}
                        className="btn btn-danger btn-sm"
                      >
                        Excluir
                      </button>
                    </div>

                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
}
