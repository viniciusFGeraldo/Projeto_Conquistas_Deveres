# Projeto_Conquistas_Deveres

minhas expecificações do sistema são essas:
funcionario:
	-nome: string
	-foto: jgep,jpg e pdf(imagem)

classe:Projeto
	-nome do projeto:string
	-responsavel:(objeto de funcionario)
		-foto
		-nome
	-sub-responsavel:(objeto de funcionario) 
		-foto
		-nome
	-resultado:string
	-entrega1:string
	-entrega2:string
	-entrega3:string
	-nota:decimal

classe:escalaRobo
	title:"escala do robô"
	mesAtual: mm/aaaa
	funcionario:(objeto de funcionario) 
		-foto
		-nome
	datas:[]:array de datas

metodos:
	-deve criar,ler,alterar e deletar funcionario.
	-deve criar,ler,alterar e deletar projeto.
	-no projeto resultado,entrega1,entrega2 entrega3 e nota é opcional
	-deve criar,ler,alterar e deletar escalaRobo.
	-no escala robô o mesAtual deve mudar sempre que virar o mês,
mostrando o mês vigente.
	-no datas quero que mostre o calendario e seja possivel escolher os 
finais de semana e feriados com base no brasil,se não contar como feriado ou 
final de semana confirmar antes se o usuario quer mesmo incluir a data mesmo 
sendo dia utel.
	-nas datas cada funcionario terá seu array de datas com a sua propria escala.