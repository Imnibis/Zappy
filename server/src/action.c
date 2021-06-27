/*
** EPITECH PROJECT, 2021
** zappy
** File description:
** action
*/

#include "server.h"

void do_actions(server_t *s, map_t *m)
{
    switch (s->acs->dur) {
        case LOOK:
            look(s, m);
            break;
        case FORWARD:
            forward(s, m);
            break;
        case LEFT:
            left(s);
            break;
        case RIGHT:
            right(s);
            break;
        case INVENTORY:
            request_inventory(s);
            break;
        case TAKE:
            take(s, m);
            break;
        case SET:
            set(s, m);
            break;
        default:
            break;
    }
}

void actions(server_t *s, map_t *m)
{
    clock_t new;
    action_t *head = s->acs;

    for (; s->acs != NULL; s->acs = s->acs->next) {
        new = clock() - s->acs->begin;
        if (new / CLOCKS_PER_SEC >= get_durations(s->acs->dur) / s->freq) {
            if (s->acs->dur != DEATH && s->acs->dur != REFILL && s->acs->dur != FOOD) {
                do_actions(s, m);
                if (s->acs->next == NULL) {
                    s->acs->prev->next = NULL;
                    free(s->acs);
                } else {
                    s->acs->prev->next = s->acs->next;
                    s->acs->next->prev = s->acs->prev;
                    free(s->acs);
                }
            }
        }
    }
    s->acs = head;
}