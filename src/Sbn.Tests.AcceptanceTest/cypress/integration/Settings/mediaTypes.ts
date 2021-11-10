/// <reference types="Cypress" />
context('Media Types', () => {

  beforeEach(() => {
    cy.sbnLogin(Cypress.env('username'), Cypress.env('password'));
  });

  it('Create media type', () => {
    const name = "Test media type";

    cy.sbnEnsureMediaTypeNameNotExists(name);

    cy.sbnSection('settings');
    cy.get('li .umb-tree-root:contains("Settings")').should("be.visible");

    cy.sbnTreeItem("settings", ["Media Types"]).rightclick();

    cy.sbnContextMenuAction("action-create").click();
    cy.get('.menu-label').first().click(); // TODO: Fucked we cant use something like cy.sbnContextMenuAction("action-mediaType").click();


    //Type name
    cy.sbnEditorHeaderName(name);


    cy.get('[data-element="group-add"]').click();

    cy.get('.umb-group-builder__group-title-input').type('Group name');
    cy.get('[data-element="property-add"]').click();
    cy.get('.editor-label').type('property name');
    cy.get('[data-element="editor-add"]').click();

    //Search for textstring
    cy.get('#datatype-search').type('Textstring');

    // Choose first item
    cy.get('ul.umb-card-grid [title="Textstring"]').closest("li").click();

    // Save property
    cy.get('.btn-success').last().click();

    //Save
    cy.get('.btn-success').click();

    //Assert
    cy.sbnSuccessNotification().should('be.visible');

    //Clean up
    cy.sbnEnsureMediaTypeNameNotExists(name);
   });

});
