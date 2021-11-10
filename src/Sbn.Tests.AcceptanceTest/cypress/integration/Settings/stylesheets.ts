/// <reference types="Cypress" />
context('Stylesheets', () => {

  beforeEach(() => {
    cy.sbnLogin(Cypress.env('username'), Cypress.env('password'));
  });

  it('Create new style sheet file', () => {
    const name = "TestStylesheet";
    const fileName = name + ".css";

   cy.sbnEnsureStylesheetNameNotExists(fileName);

    cy.sbnSection('settings');
    cy.get('li .umb-tree-root:contains("Settings")').should("be.visible");

    cy.sbnTreeItem("settings", ["Stylesheets"]).rightclick();

    cy.sbnContextMenuAction("action-create").click();
    cy.get('.menu-label').first().click(); // TODO: Fucked we cant use something like cy.sbnContextMenuAction("action-mediaType").click();

    //Type name
    cy.sbnEditorHeaderName(name);

    //Save
    cy.get('.btn-success').click();

    //Assert
    cy.sbnSuccessNotification().should('be.visible');

    //Clean up
    cy.sbnEnsureStylesheetNameNotExists(fileName);
   });

});
