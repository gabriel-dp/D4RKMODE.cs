# **Código da equipe D4RKMODE - OBR Nacional**

Não é permitida de nenhuma forma a cópia do contéudo aqui presente para uso em competição. Esteja avisado.

<br/>

## **Organização**

Este projeto utiliza uma estrutura de multíplos arquivos em Rust denominada `sBotics-file-modulator`.

Dentro de `./src` estão todos os scripts responsáveis pelas ações do robô, divididos em "geral/pista/resgate/terminar"

A pasta `./out` contém o resultado da junção dos arquivos que é executado na rotina -> `final.cs`

## **Robô**

<div align="center">
<br/>
<img width=225 src="./docs/robot.png" align="center"/>

&nbsp;

### **Robô 3 (bombeiro)**

#### **Empilhadeira Frontal**
</div>

## **LED**

O LED é usado como um indicador de posição, as cores mudam a depender de onde o robô está e o que está fazendo. Abaixo está a referência das cores.

<img height="25" src="https://img.shields.io/badge/Branco%20-Normal-%23DDDDDD?labelColor=FEFFFF"/>
<img height="25" src="https://img.shields.io/badge/Preto%20-Curva/Triângulo-%23DDDDDD?labelColor=000000"/>
<img height="25" src="https://img.shields.io/badge/Verde%20-Intersecção-%23DDDDDD?labelColor=00CC00"/>
<img height="25" src="https://img.shields.io/badge/Rosa%20-Obstáculo Possível-%23DDDDDD?labelColor=D264D2"/>
<img height="25" src="https://img.shields.io/badge/Roxo%20-Obstáculo Confirmado-%23DDDDDD?labelColor=915AB4"/>
<img height="25" src="https://img.shields.io/badge/Laranja%20-Pedeu a linha-%23DDDDDD?labelColor=C88232"/>
<img height="25" src="https://img.shields.io/badge/Azul%20-Rampa/Gangorra-%23DDDDDD?labelColor=465AC8"/>
<img height="25" src="https://img.shields.io/badge/Ciano%20-Vítima-%23DDDDDD?labelColor=00c8c8"/>
<img height="25" src="https://img.shields.io/badge/Amarelo%20-Parede-%23DDDDDD?labelColor=ffff00"/>
<img height="25" src="https://img.shields.io/badge/Verde Escuro%20-Saída-%23DDDDDD?labelColor=009900"/>
<img height="25" src="https://img.shields.io/badge/Vermelho%20-Fim-%23DDDDDD?labelColor=ff0000"/>

## **Console**

O Console também é usado para indicar o que o robô está fazendo. Algumas informações relevantes:

<div align="center">
	Indicação de local <b>(Pista)</b> e Informação extra <b>(Velocidade dos motores)</b>
	<img width=750 src="./docs/console1.png"/>
	<br/>
	Indicação de local <b>(Pista)</b> e Ação <b>(Intersecção)</b>
	<img width=750 src="./docs/console2.png"/>
	<br/>
	Indicação de local <b>(Resgate)</b>, Ação <b>(Vítima detectada)</b> e Informação extra <b>(Carregando vítima)</b>
	<img width=750 src="./docs/console3.png"/>
	<br/>
	E um pouco de diversão haha
	<img width=750 src="./docs/console4.png"/>
</div>
