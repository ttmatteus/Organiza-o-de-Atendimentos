# Solução: Organização de Atendimentos em Clínica Veterinária

Implementação em **C#** (.NET 8) de um sistema de organização de atendimentos utilizando a heurística **First Fit Decreasing (FFD)**.

---

# Requisitos

* .NET 8.0 SDK ou superior
* Visual Studio 2022, Rider ou VS Code

---

# Instalação

```bash
dotnet restore
dotnet build
dotnet test
```

---

# Execução

```bash
dotnet run
```

ou

```bash
dotnet run -- atendimentos.txt
```

---

# Formato da Entrada

Arquivo de entrada:

```txt
Castração de gato adulto 90min
Aplicação de vacina antirrábica expresso
Limpeza dentária em cão de pequeno porte 45min
```

## Regras

* Um atendimento por linha
* Formato:

```txt
[nome] [duração]
```

* A duração pode ser:

  * `XXmin`
  * `expresso` (10 minutos)

---

# Formato da Saída

```txt
Consultórios necessários: 4

Consultório 1:

Turno da manhã:
08:00 Cirurgia ortopédica em cão atropelado
10:00 Castração de gato adulto
11:30 Higienização

Turno da tarde:
13:30 Ultrassonografia abdominal
14:30 Avaliação cardiológica
15:30 Reunião de encerramento
```

---

# Testes

Os testes validam:

* parsing dos atendimentos;
* cálculo correto de horários;
* limite de capacidade das sessões;
* criação dinâmica de consultórios;
* funcionamento do algoritmo FFD;
* organização correta dos atendimentos.

Executar:

```bash
dotnet test
```

---

# Decisões Técnicas

## Linguagem

Foi utilizado C# por conta de:

* tipagem forte;
* boa organização orientada a objetos;
* suporte moderno do .NET;
* facilidade para encapsular regras do sistema;
* e ótima legibilidade.

---

## Estratégia Utilizada

O algoritmo escolhido foi:

### First Fit Decreasing (FFD)

Funcionamento:

1. ordena os atendimentos do maior para o menor;
2. tenta encaixar no primeiro consultório disponível;
3. se não houver espaço, cria um novo consultório.

---

## Complexidade

### Tempo

[
O(n \log n)
]

na maioria dos casos.

Pior caso:

[
O(n^2)
]

---

## Limitações

* não garante solução ótima;
* não possui persistência em banco;
* não considera especialização de veterinários;
* trabalha apenas com saída em terminal.

---

# Possíveis Melhorias Futuras

* API REST
* interface web
* exportação PDF
* importação CSV/Excel
* múltiplas estratégias de escalonamento
* persistência com banco de dados

---

# Autor

Matteus
