import pygame
import sys
import random

pygame.init()
screen = pygame.display.set_mode((450, 450))
pygame.display.set_caption("Tic Tac Toe")

fondo = pygame.image.load("static/fondo.png")
circulo = pygame.image.load("static/circle.png")
equis = pygame.image.load("static/x.png")

fondo = pygame.transform.scale(fondo, (450, 450))
circulo = pygame.transform.scale(circulo, (125, 125))
equis = pygame.transform.scale(equis, (125, 125))

# --- Constantes y Variables del Juego ---

# --- Clase para la caja de texto ---
class InputBox:
    def __init__(self, x, y, w, h, text=''):
        self.rect = pygame.Rect(x, y, w, h)
        self.color_inactive = pygame.Color('lightskyblue3')
        self.color_active = pygame.Color('dodgerblue2')
        self.color = self.color_inactive
        self.text = text
        self.font = pygame.font.Font(None, 32)
        self.active = False

    def handle_event(self, event):
        if event.type == pygame.MOUSEBUTTONDOWN:
            if self.rect.collidepoint(event.pos):
                self.active = not self.active
            else:
                self.active = False
            self.color = self.color_active if self.active else self.color_inactive
        if event.type == pygame.KEYDOWN:
            if self.active:
                if event.key == pygame.K_RETURN:
                    self.active = False
                    self.color = self.color_inactive
                elif event.key == pygame.K_BACKSPACE:
                    self.text = self.text[:-1]
                else:
                    self.text += event.unicode

    def draw(self, screen):
        txt_surface = self.font.render(self.text, True, (255, 255, 255))
        screen.blit(txt_surface, (self.rect.x + 5, self.rect.y + 5))
        pygame.draw.rect(screen, self.color, self.rect, 2)

# Coordenadas corregidas para centrar las imágenes en cada celda de 150x150
coor = [[(12, 12), (162, 12), (312, 12)],
        [(12, 162), (162, 162), (312, 162)],
        [(12, 312), (162, 312), (312, 312)]]

tablero = [['', '', ''],
           ['', '', ''],
           ['', '', '']]

turno = 'X'
winner = None  # Puede ser 'X', 'O', o 'EMPATE'
player1_name = "Jugador 1"
player2_name = "Jugador 2"

# Estados del juego: 'MENU', 'NAME_INPUT', 'JUGANDO', 'FIN_PARTIDA'
game_state = 'MENU'
game_mode = None  # '1v1' o 'CPU'

clock = pygame.time.Clock()
font = pygame.font.Font(None, 60)

input_box1 = InputBox(125, 150, 200, 32)
input_box2 = InputBox(125, 250, 200, 32)

# --- Funciones del Juego ---

def reset_game():
    """Reinicia el tablero y las variables para una nueva partida."""
    global tablero, turno, winner
    tablero = [['', '', ''], ['', '', ''], ['', '', '']]
    turno = 'X'
    winner = None

def dibujar_board():
    """Dibuja el tablero y las piezas (X y O)."""
    screen.blit(fondo, (0, 0))
    for fila in range(3):
        for col in range(3):
            if tablero[fila][col] == 'X':
                dibujar_x(fila, col)
            elif tablero[fila][col] == 'O':
                dibujar_o(fila, col)

def dibujar_x(fila, col):
    """Dibuja una X en la posición dada."""
    screen.blit(equis, coor[fila][col])

def dibujar_o(fila, col):
    """Dibuja una O en la posición dada."""
    screen.blit(circulo, coor[fila][col])

def check_game_status():
    """Verifica si hay un ganador o si es un empate. Devuelve 'X', 'O', 'EMPATE' o None."""
    for fila in range(3):
        if tablero[fila][0] == tablero[fila][1] == tablero[fila][2] and tablero[fila][0] != '':
            return tablero[fila][0] # Devuelve 'X' o 'O'
    for col in range(3):
        if tablero[0][col] == tablero[1][col] == tablero[2][col] and tablero[0][col] != '':
            return tablero[0][col]
    if tablero[0][0] == tablero[1][1] == tablero[2][2] and tablero[0][0] != '':
        return tablero[0][0]
    if tablero[0][2] == tablero[1][1] == tablero[2][0] and tablero[0][2] != '':
        return tablero[0][2]
    
    # Verificar si hay empate (si no hay casillas vacías)
    if all(tablero[row][col] != '' for row in range(3) for col in range(3)):
        return 'EMPATE'

    return None # El juego continúa

def cpu_move():
    """La CPU hace un movimiento aleatorio en una casilla vacía."""
    global turno
    celdas_vacias = []
    for fila in range(3):
        for col in range(3):
            if tablero[fila][col] == '':
                celdas_vacias.append((fila, col))
    
    if celdas_vacias:
        fila, col = random.choice(celdas_vacias)
        tablero[fila][col] = 'O'
        turno = 'X'

# Definir rects para los botones del menú para que estén disponibles en todo el bucle
button_1v1_rect = pygame.Rect(100, 150, 250, 60)
button_cpu_rect = pygame.Rect(100, 250, 250, 60)


# --- Bucle Principal del Juego ---
while True:
    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            pygame.quit()
            sys.exit()

        if game_state == 'NAME_INPUT':
            input_box1.handle_event(event)
            if game_mode == '1v1':
                input_box2.handle_event(event)

        if event.type == pygame.MOUSEBUTTONDOWN:
            mouseX, mouseY = event.pos

            if game_state == 'MENU':
                # Lógica para los botones del menú
                if button_1v1_rect.collidepoint(mouseX, mouseY):
                    game_mode = '1v1'
                    game_state = 'NAME_INPUT'
                    input_box1.text = ''
                    input_box2.text = ''
                elif button_cpu_rect.collidepoint(mouseX, mouseY):
                    game_mode = 'CPU'
                    game_state = 'NAME_INPUT'
                    input_box1.text = ''
                    input_box2.text = 'CPU'

            elif game_state == 'NAME_INPUT':
                start_button_rect = pygame.Rect(150, 350, 150, 50)
                if start_button_rect.collidepoint(mouseX, mouseY):
                    player1_name = input_box1.text if input_box1.text else "Jugador 1"
                    if game_mode == '1v1':
                        player2_name = input_box2.text if input_box2.text else "Jugador 2"
                    else:
                        player2_name = "CPU"
                    reset_game()
                    game_state = 'JUGANDO'

            elif game_state == 'JUGANDO':
                # Lógica de clic durante la partida
                fila = mouseY // 150
                col = mouseX // 150
                if tablero[fila][col] == '':
                    # Movimiento del jugador humano
                    tablero[fila][col] = turno
                    turno = 'O' if turno == 'X' else 'X'

                    winner = check_game_status()
                    if winner:
                        game_state = 'FIN_PARTIDA'

            elif game_state == 'FIN_PARTIDA':
                # Si se hace clic en la pantalla de fin de partida, vuelve al menú
                game_state = 'MENU'

    # --- Lógica de Estados del Juego ---
    if game_state == 'MENU':
        screen.fill((0, 10, 20)) # Fondo oscuro para el menú
        pygame.draw.rect(screen, (0, 100, 150), button_1v1_rect)
        pygame.draw.rect(screen, (0, 100, 150), button_cpu_rect)

        text_1v1 = font.render("1 vs 1", True, (255, 255, 255))
        text_cpu = font.render("vs CPU", True, (255, 255, 255))
        screen.blit(text_1v1, (160, 160))
        screen.blit(text_cpu, (150, 260))

    elif game_state == 'NAME_INPUT':
        screen.fill((0, 10, 20))
        label_font = pygame.font.Font(None, 36)

        label1 = label_font.render("Jugador 1 (X):", True, (255, 255, 255))
        screen.blit(label1, (125, 120))
        input_box1.draw(screen)

        if game_mode == '1v1':
            label2 = label_font.render("Jugador 2 (O):", True, (255, 255, 255))
            screen.blit(label2, (125, 220))
            input_box2.draw(screen)

        start_button_rect = pygame.Rect(150, 350, 150, 50)
        pygame.draw.rect(screen, (0, 150, 100), start_button_rect)
        start_text = font.render("Jugar", True, (255, 255, 255))
        screen.blit(start_text, (start_button_rect.x + 15, start_button_rect.y + 5))

    elif game_state == 'JUGANDO':
        dibujar_board()

        current_player_name = player1_name if turno == 'X' else player2_name
        turn_text = pygame.font.Font(None, 30).render(f"Turno: {current_player_name}", True, (255, 255, 0))
        screen.blit(turn_text, (10, 10))

        # Si es el turno de la CPU y el juego no ha terminado
        if game_mode == 'CPU' and turno == 'O' and not winner:
            cpu_move()
            winner = check_game_status()
            if winner:
                game_state = 'FIN_PARTIDA'

    elif game_state == 'FIN_PARTIDA':
        dibujar_board() # Muestra el tablero final

        # Muestra el mensaje de ganador o empate
        if winner == 'EMPATE':
            msg = "¡Es un empate!"
        else:
            winner_name = player1_name if winner == 'X' else player2_name
            msg = f"¡Ganador: {winner_name}!"

        text = font.render(msg, True, (255, 255, 0))
        text_rect = text.get_rect(center=(450/2, 450/2))
        pygame.draw.rect(screen, (0, 0, 0, 150), text_rect.inflate(20, 20)) # Fondo semitransparente
        screen.blit(text, text_rect)

    pygame.display.update()
    clock.tick(30)
