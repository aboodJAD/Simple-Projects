<%@page import="java.util.*"%>
<%@page import="ModelClasses.*"%>
<%@page contentType="text/html" pageEncoding="UTF-8"%>
<!DOCTYPE html>
<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <title>Previous Games</title>
    </head>
    <body>
        <jsp:useBean id="games" class="ModelClasses.GameCollector" scope="session"/>
        <%
            ArrayList<EndedGame> savedGames = games.getAllGames();
        %>
        <%for(EndedGame g:savedGames){%>
            <%
                String xHeader="Player X : "+g.getXPlayer(),
                    oHeader="Player O : "+g.getOPlayer();
                Game game=g.getGame();
            %>
            
            <%if(game.getWinState()==Value.EMP){%>
                <p>Game ended with tie</p><br>
            <%}else if(game.getWinState()==Value.X){
                xHeader="<strong>"+xHeader+" (Winner)</Strong>";
            }else {
                oHeader="<strong>"+oHeader+" (Winner)</Strong>";
            }%>
        <p><%=xHeader%>&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp
                <%=oHeader%></p>
            <%request.setAttribute("game",g.getGame());%>
            <jsp:include page="GameDraw.jsp" flush="true"/>
            <br>    
        <%}%>
    </body>
</html>
