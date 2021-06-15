##
## EPITECH PROJECT, 2021
## PROJECT
## File description:
## Makefile
##

NAME		=	../zappy_server

LD			=	@ld
CC			=	@gcc
DIROBJ		=	objects
EXTENSION	=	.c

SRC			=	$(wildcard $(addprefix lib/, *)$(EXTENSION))
SRC			+=	$(wildcard $(addprefix src/, *)$(EXTENSION))
NOM			=	$(basename $(notdir $(SRC)))
OBJ			=	$(addprefix $(DIROBJ)/, $(addsuffix .o, $(NOM)))
SRC-CRIT	=	$(wildcard $(addprefix tests/, *)$(EXTENSION))
OBJ-CRIT	=	$(SRC-CRIT:$(EXTENSION)=.o)

CPPFLAGS	+=	-Wall -Wextra
LDFLAGS		+=	-g3
CFLAGS		+=	-Iinclude -fno-builtin -g3
CXXFLAGS	+=	-IInclude -fno-builtin
CRITFLAGS	=	-lcriterion --coverage
CGRAPHICS	=	-lcsfml-graphics -lcsfml-audio -lcsfml-window -lcsfml-system

END			=	\033[0m
BOLD		=	\033[1m
GREY		=	\033[30m
RED			=	\033[31m
GREEN		=	\033[32m
YELLOW		=	\033[33m
BLUE		=	\033[34m
PURPLE		=	\033[35m
CYAN		=	\033[36m
WHITE		=	\033[37m

all:	$(NAME)

$(NAME):	$(OBJ)
	$(CC) -o $(NAME) $(OBJ) -luuid $(CFLAGS)
	@echo "$(GREEN)=========( COMPLETED )=========$(END)"

test_run:	$(OBJ-CRIT)
	@$(CC) $(CFLAGS) $(CRITFLAGS) $(OBJ-CRIT)
	@echo "$(GREEN)=========( COMPLETED )=========$(END)"
	./a.out

clean:
	@find . -type f \( -iname "*~" \) -delete
	@$(RM) $(OBJ)
	@$(RM) $(OBJ-CRIT)
	@$(RM) $(NAME)
	@$(RM) -r $(DIROBJ)
	@echo "$(RED)=========( CLEANED )=========$(END)"

fclean:	clean
	@$(RM) -r $(DIROBJ)
	@$(RM) ../$(NAME)
	@$(RM) a.out

re:	fclean all

$(DIROBJ):
	@mkdir -p $@ \
	&& echo "$(GREEN)[OK]\t$(BLUE)Creating:$(WHITE)$(BOLD)" $(DIROBJ) "$(END)" \
	|| echo "$(RED)[KO]\tCan't create:$(BOLD)" $(DIROBJ) "$(END)"

$(DIROBJ)/%.o: lib/%$(EXTENSION) | $(DIROBJ)
	@$(CC) $(CFLAGS) $(CPPFLAGS) -c -o $@ $< \
	&& echo "$(GREEN)[OK]\t$(BLUE)Compiling:$(WHITE)$(BOLD)" $< "$(END)" \
	|| echo "$(RED)[KO]\tCan't compile:$(BOLD)" $< "$(END)"

$(DIROBJ)/%.o:	src/%$(EXTENSION) | $(DIROBJ)
	@$(CC) $(CFLAGS) $(CPPFLAGS) -c -o $@ $< \
	&& echo "$(GREEN)[OK]\t$(BLUE)Compiling:$(WHITE)$(BOLD)" $< "$(END)" \
	|| echo "$(RED)[KO]\tCan't compile:$(BOLD)" $< "$(END)"

.PHONY: all test_run clean fclean re