/// <reference types="Cypress" />
context('Members', () => {

    beforeEach(() => {
        cy.sbnLogin(Cypress.env('username'), Cypress.env('password'));
    });

    it('Create member', () => {
        const name = "Alice Bobson";
        const email = "alice-bobson@acceptancetest.sbn";
        const password = "$AUlkoF*St0kgPiyyVEk5iU5JWdN*F7&";
        const passwordTimeout = 20000

        cy.sbnEnsureMemberEmailNotExists(email);
        cy.sbnSection('member');
        cy.get('li .umb-tree-root:contains("Members")').should("be.visible");

        cy.sbnTreeItem("member", ["Members"]).rightclick();

        cy.sbnContextMenuAction("action-create").click();
        cy.get('.menu-label').first().click();

        //Type name
        cy.sbnEditorHeaderName(name);

        cy.get('input#_umb_login').clear().type(email);
        cy.get('input#_umb_email').clear().type(email);
        cy.get('input#password').clear().type(password, { timeout: passwordTimeout });
        cy.get('input#confirmPassword').clear().type(password, { timeout: passwordTimeout });

        // Save
        cy.get('.btn-success').click();

        //Assert
        cy.sbnSuccessNotification().should('be.visible');

        //Clean up
        cy.sbnEnsureMemberEmailNotExists(email);

    });

});
