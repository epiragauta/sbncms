context('Member Groups', () => {

    beforeEach(() => {
        cy.sbnLogin(Cypress.env('username'), Cypress.env('password'));
    });

    it('Create member group', () => {
        const name = "Test Group";

        cy.sbnEnsureMemberGroupNameNotExists(name);

        cy.sbnSection('member');
        cy.get('li .umb-tree-root:contains("Members")').should("be.visible");

        cy.sbnTreeItem("member", ["Member Groups"]).rightclick();

        cy.sbnContextMenuAction("action-create").click();

        //Type name
        cy.sbnEditorHeaderName(name);

        // Save
        cy.get('.btn-success').click();

        //Assert
        cy.sbnSuccessNotification().should('be.visible');

        //Clean up
        cy.sbnEnsureMemberGroupNameNotExists(name);
    });

});
