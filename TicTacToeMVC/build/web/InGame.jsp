<%@page import="ModelClasses.*"%>
<%@page contentType="text/html" pageEncoding="UTF-8"%>
<!DOCTYPE html>
<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <title>Move !</title>
    </head>
    <body>
        <%
            String xPlayer,oPlayer;
            xPlayer=(String)session.getAttribute("xplayer");
            oPlayer=(String)session.getAttribute("oplayer");
            Game game=(Game)session.getAttribute("game");
        %>
        <%if(request.getAttribute("errormessage")!=null){%>
            <p><%=request.getAttribute("errormessage")%></p><br>
        <%}%>
        <p>Player X : <%=xPlayer%>&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp
            Player O : <%=oPlayer%>
        </p>
        <form method="GET" action="Controller">
            <%request.setAttribute("game",game);%>
            <jsp:include page="GameDraw.jsp" />
            <br>
            <input type="submit" name="confirm" value="Confirm">
            <br><br>
            <input type="submit" name="history" value="Previous Games">
        </form>
    </body>
</html>
