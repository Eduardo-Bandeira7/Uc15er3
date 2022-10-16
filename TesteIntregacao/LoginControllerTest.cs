using ChapterBET6.Controllers;
using ChapterBET6.Interfaces;
using ChapterBET6.Models;
using ChapterBET6.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TesteIntregacao
{
    public class LoginControllerTest
    {
        [Fact]
        public void LoginController_Retornar_Usuario_Invalido()
        {
            //Arrange - preparacao
            var RepositoryEspelhado = new Mock<IUsuarioRepository>();

            RepositoryEspelhado.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns((Usuario)null);

            var controller = new LoginController(RepositoryEspelhado.Object);

            LoginViewModel dadosUsuario = new LoginViewModel();
            dadosUsuario.email = "brigadeiro@email.com";
            dadosUsuario.senha = "lasanha";


            //ACT - Acao
            var resultado = controller.Login(dadosUsuario);


            //Assert - Verificacao
            Assert.IsType<UnauthorizedObjectResult>(resultado);
        }


        [Fact]
        public void LoginController_Retornar_Token()
        {
            //Arrange - Preparação
            Usuario UsuarioRetornado = new Usuario();
            UsuarioRetornado.Email = "churrasco@email.com";
            UsuarioRetornado.Senha = "cerveja";
            UsuarioRetornado.Tipo = "0";
            UsuarioRetornado.Id = 1;


            var RepositoryEspelhado = new Mock<IUsuarioRepository>();

            RepositoryEspelhado.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>()));

            LoginViewModel dadosUsuario = new LoginViewModel();
            dadosUsuario.email = "brigadeiro@email.com";
            dadosUsuario.senha = "lasanha";

            string issuerValido = "chapter.webapi";

            //Act - Ação
            OkObjectResult resultado = (OkObjectResult)controller.Login(dadosUsuario);

            string tokenString = resultado.Value.ToString().Split(" ")[3];

            var jwtHandler = new JwtSecurityHandler();
            var tokenJwt = jwtHandler.ReadJwtToken(tokenString);


            //Assert - Verificação
            Assert.Equal(issuerValido, tokenJwt.Issuer);


        }



    }
}