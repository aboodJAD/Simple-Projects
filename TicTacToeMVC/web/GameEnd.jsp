<%@page import="ModelClasses.*"%>
<%@page contentType="text/html" pageEncoding="UTF-8"%>
<!DOCTYPE html>
<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <title>End !</title>
    </head>
    <body>
        <%
            Game game=(Game)session.getAttribute("game");
            GameCollector games=(GameCollector)session.getAttribute("games");
            if(games==null)
                   games=new GameCollector();
            games.addGame(game,(String)session.getAttribute("xplayer"),
                    (String)session.getAttribute("oplayer"));  
            session.setAttribute("games",games);
            Value whoWin=game.getWinState();
        %>
        <%if(whoWin==Value.EMP){%>
            <p>Game end with tie<br></p>
            <p>Player X : <%=(String)session.getAttribute("xplayer")%>
                &nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp
                Player O : <%=(String)session.getAttribute("oplayer")%>
            </p>            
        <%}else if(whoWin==Value.X){%>
                <p><strong>Player X : <%=(String)session.getAttribute("xplayer")%>(Winner)</strong>
                    &nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp
                    Player O : <%=(String)session.getAttribute("oplayer")%>
                </p>            
        <%}else{%>
                <p>Player X : <%=(String)session.getAttribute("xplayer")%>
                    &nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp
                    <strong>Player O : <%=(String)session.getAttribute("oplayer")%>(Winner)</strong>
                </p>            
        <%}%>
        <jsp:include page="GameDraw.jsp" />        
        <form method="GET" action="Controller">
            <%request.setAttribute("game",game);%>
            <br>
            <input type="submit" name="newgame" value="New Game" >     
            <br><br>
            <input type="submit" name="history" value="Previous Games"> 
        </form>
    </body>
</html>
