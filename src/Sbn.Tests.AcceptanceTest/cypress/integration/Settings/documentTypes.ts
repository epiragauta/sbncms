/// <reference types="Cypress" />
import { DocumentTypeBuilder } from 'sbn-cypress-testhelpers';
context('Document Types', () => {

  beforeEach(() => {
    cy.sbnLogin(Cypress.env('username'), Cypress.env('password'));
  });

  it('Create document type', () => {
    const name = "Test document type";

    cy.sbnEnsureDocumentTypeNameNotExists(name);
    cy.sbnEnsureTemplateNameNotExists(name);

    cy.sbnSection('settings');
    cy.get('li .umb-tree-root:contains("Settings")').should("be.visible");

    cy.sbnTreeItem("settings", ["Document Types"]).rightclick();

    cy.sbnContextMenuAction("action-create").click();
    cy.sbnContextMenuAction("action-documentType").click();
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
    cy.sbnEnsureTemplateNameNotExists(name);

    //Clean up
    cy.sbnEnsureDocumentTypeNameNotExists(name);
   });

  it('Delete document type', () => {
    const name = "Test document type";
    cy.sbnEnsureDocumentTypeNameNotExists(name);

    const dataType = new DocumentTypeBuilder()
      .withName(name)
      .build();

    cy.saveDocumentType(dataType);


    cy.sbnSection('settings');
    cy.get('li .umb-tree-root:contains("Settings")').should("be.visible");

    cy.sbnTreeItem("settings", ["Document Types", name]).rightclick();

    cy.sbnContextMenuAction("action-delete").click();

    cy.get('label.checkbox').click();
    cy.sbnButtonByLabelKey("delete").click();

    cy.contains(name).should('not.exist');

    cy.sbnEnsureDocumentTypeNameNotExists(name);


  });
});
