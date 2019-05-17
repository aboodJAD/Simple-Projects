package CntrollerServlets;

import ModelClasses.*;
import java.io.IOException;
import java.io.PrintWriter;
import javax.servlet.RequestDispatcher;
import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

@WebServlet(name = "Controller", urlPatterns = {"/Controller"})
public class Controller extends HttpServlet {

    protected void processRequest(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        response.setContentType("text/html;charset=UTF-8");
        HttpSession session=request.getSession();        
        try (PrintWriter out = response.getWriter()) {
            RequestDispatcher rq = request.getRequestDispatcher("index.html");;
            if(request.getParameter("start")!=null){
                String xPlayer,oPlayer;
                int size,reqToWin;
                xPlayer=(String)request.getParameter("xplayer");
                oPlayer=(String)request.getParameter("oplayer");
                size=Integer.parseInt(request.getParameter("rbyr"));
                reqToWin=Integer.parseInt(request.getParameter("reqtowin"));
                Game game=new Game(size,reqToWin);
                session.setAttribute("xplayer",xPlayer);
                session.setAttribute("oplayer",oPlayer);
                session.setAttribute("game",game);
                rq=request.getRequestDispatcher("InGame.jsp");
            }else if(request.getParameter("confirm")!=null){
                rq=request.getRequestDispatcher("ProcessServlet");
            }else if(request.getParameter("history")!=null){
                rq=request.getRequestDispatcher("HistoryJsp.jsp");
            }else if(request.getParameter("newgame")!=null){
                rq=request.getRequestDispatcher("index.html");
            }
            rq.forward(request, response);
        }
    }

    // <editor-fold defaultstate="collapsed" desc="HttpServlet methods. Click on the + sign on the left to edit the code.">
    /**
     * Handles the HTTP <code>GET</code> method.
     *
     * @param request servlet request
     * @param response servlet response
     * @throws ServletException if a servlet-specific error occurs
     * @throws IOException if an I/O error occurs
     */
    @Override
    protected void doGet(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        processRequest(request, response);
    }

    /**
     * Handles the HTTP <code>POST</code> method.
     *
     * @param request servlet request
     * @param response servlet response
     * @throws ServletException if a servlet-specific error occurs
     * @throws IOException if an I/O error occurs
     */
    @Override
    protected void doPost(HttpServletRequest request, HttpServletResponse response)
            throws ServletException, IOException {
        processRequest(request, response);
    }

    /**
     * Returns a short description of the servlet.
     *
     * @return a String containing servlet description
     */
    @Override
    public String getServletInfo() {
        return "Short description";
    }// </editor-fold>

}
