context('User Groups', () => {

  beforeEach(() => {
    cy.sbnLogin(Cypress.env('username'), Cypress.env('password'));
  });

  it('Create user group', () => {
    const name = "Test Group";

    cy.sbnEnsureUserGroupNameNotExists(name);

    cy.sbnSection('users');
    cy.get('[data-element="sub-view-userGroups"]').click();

    cy.sbnButtonByLabelKey("actions_createGroup").click();

    //Type name
    cy.sbnEditorHeaderName(name);

    // Assign sections
    cy.get('.umb-box:nth-child(1) .umb-property:nth-child(1) localize').click();
    cy.get('.umb-tree-item__inner').click({multiple:true, timeout: 10000});
    cy.get('.btn-success').last().click();

    // Save
    cy.get('.btn-success').click();

    //Assert
    cy.sbnSuccessNotification().should('be.visible');

    //Clean up
    cy.sbnEnsureUserGroupNameNotExists(name);
   });

});
